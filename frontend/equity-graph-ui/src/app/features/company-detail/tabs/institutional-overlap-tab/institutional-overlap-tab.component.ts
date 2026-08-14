import { Component, Input, OnChanges, SimpleChanges, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { EquityGraphApiService } from '../../../../core/services/equity-graph-api.service';
import { InstitutionalOverlapEntry } from '../../../../core/models/institutional-overlap';
import { CompanySummary } from '../../../../core/models/company';
import { LoadingSkeletonComponent } from '../../../../shared/components/loading-skeleton/loading-skeleton.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { ErrorStateComponent } from '../../../../shared/components/error-state/error-state.component';

@Component({
  selector: 'app-institutional-overlap-tab',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    LoadingSkeletonComponent,
    EmptyStateComponent,
    ErrorStateComponent
  ],
  templateUrl: './institutional-overlap-tab.component.html',
  styleUrl: './institutional-overlap-tab.component.scss'
})
export class InstitutionalOverlapTabComponent implements OnChanges {
  private readonly apiService = inject(EquityGraphApiService);

  @Input({ required: true }) companyId!: string;
  @Input({ required: true }) companyName!: string;
  @Input() active: boolean = false;

  // Comparison State
  readonly selectedCompany = signal<CompanySummary | null>(null);
  readonly overlapEntries = signal<InstitutionalOverlapEntry[]>([]);
  readonly isLoading = signal<boolean>(false);
  readonly errorMessage = signal<string | null>(null);

  // Search for comparison company
  readonly companySearchTerm = signal<string>('');
  readonly searchResults = signal<CompanySummary[]>([]);
  readonly isSearching = signal<boolean>(false);
  readonly isDropdownOpen = signal<boolean>(false);

  private readonly searchSubject$ = new Subject<string>();
  private searchSubscription?: Subscription;

  // Cache overlap query by target company ID
  private overlapCache = new Map<string, InstitutionalOverlapEntry[]>();

  constructor() {
    this.searchSubscription = this.searchSubject$
      .pipe(
        debounceTime(250),
        distinctUntilChanged(),
        switchMap((term) => {
          if (!term.trim()) {
            return this.apiService.getCompanies();
          }
          return this.apiService.getCompanies(term.trim());
        })
      )
      .subscribe({
        next: (companies) => {
          // Filter out current company
          const filtered = companies.filter((c) => c.id !== this.companyId);
          this.searchResults.set(filtered);
          this.isSearching.set(false);
        },
        error: () => {
          this.isSearching.set(false);
        }
      });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['companyId'] && !changes['companyId'].firstChange) {
      this.selectedCompany.set(null);
      this.overlapEntries.set([]);
      this.overlapCache.clear();
    }

    if (this.active && this.searchResults().length === 0) {
      this.loadInitialCompanySuggestions();
    }
  }

  loadInitialCompanySuggestions(): void {
    this.apiService.getCompanies().subscribe({
      next: (companies) => {
        const filtered = companies.filter((c) => c.id !== this.companyId);
        this.searchResults.set(filtered);
      }
    });
  }

  onSearchInput(term: string): void {
    this.companySearchTerm.set(term);
    this.isSearching.set(true);
    this.isDropdownOpen.set(true);
    this.searchSubject$.next(term);
  }

  onSelectCompany(company: CompanySummary): void {
    this.selectedCompany.set(company);
    this.companySearchTerm.set(company.name);
    this.isDropdownOpen.set(false);
    this.fetchOverlap(company.id);
  }

  onClearSelection(): void {
    this.selectedCompany.set(null);
    this.companySearchTerm.set('');
    this.overlapEntries.set([]);
    this.loadInitialCompanySuggestions();
  }

  fetchOverlap(targetCompanyId: string): void {
    if (this.overlapCache.has(targetCompanyId)) {
      this.overlapEntries.set(this.overlapCache.get(targetCompanyId)!);
      this.isLoading.set(false);
      this.errorMessage.set(null);
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.apiService.getInstitutionalOverlap(this.companyId, targetCompanyId).subscribe({
      next: (data) => {
        this.overlapCache.set(targetCompanyId, data);
        this.overlapEntries.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.isLoading.set(false);
        this.errorMessage.set(err.message || 'Failed to compare institutional holders.');
      }
    });
  }
}
