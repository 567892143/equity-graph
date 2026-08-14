import { Component, OnInit, OnDestroy, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { EquityGraphApiService } from '../../core/services/equity-graph-api.service';
import { CompanySummary } from '../../core/models/company';
import { CompanyCardComponent } from './components/company-card/company-card.component';
import { LoadingSkeletonComponent } from '../../shared/components/loading-skeleton/loading-skeleton.component';
import { EmptyStateComponent } from '../../shared/components/empty-state/empty-state.component';
import { ErrorStateComponent } from '../../shared/components/error-state/error-state.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    CompanyCardComponent,
    LoadingSkeletonComponent,
    EmptyStateComponent,
    ErrorStateComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit, OnDestroy {
  private readonly apiService = inject(EquityGraphApiService);
  private readonly router = inject(Router);

  // Component Reactive State
  readonly companies = signal<CompanySummary[]>([]);
  readonly isLoading = signal<boolean>(true);
  readonly errorMessage = signal<string | null>(null);
  readonly searchTerm = signal<string>('');
  readonly selectedSector = signal<string | null>(null);

  // Available Sectors for Filtering
  readonly sectors: string[] = [
    'Information Technology',
    'Automotive',
    'Financial Services',
    'Automotive Components',
    'Semiconductors',
    'FMCG',
    'Pharma',
    'Energy'
  ];

  // Search & Filter debouncing stream
  private readonly filterQuery$ = new Subject<{ search: string; sector: string | null }>();
  private filterSubscription?: Subscription;

  ngOnInit(): void {
    this.setupSearchPipeline();
    // Initial fetch on mount
    this.triggerFetch();
  }

  ngOnDestroy(): void {
    this.filterSubscription?.unsubscribe();
  }

  private setupSearchPipeline(): void {
    this.filterSubscription = this.filterQuery$
      .pipe(
        debounceTime(300),
        distinctUntilChanged((prev, curr) => prev.search === curr.search && prev.sector === curr.sector),
        switchMap(({ search, sector }) => {
          this.isLoading.set(true);
          this.errorMessage.set(null);
          return this.apiService.getCompanies(
            search.trim() || undefined,
            sector || undefined
          );
        })
      )
      .subscribe({
        next: (data) => {
          this.companies.set(data);
          this.isLoading.set(false);
          this.errorMessage.set(null);
        },
        error: (err) => {
          this.isLoading.set(false);
          this.errorMessage.set(err.message || 'Failed to load companies. Please check your connection.');
        }
      });
  }

  private triggerFetch(): void {
    this.filterQuery$.next({
      search: this.searchTerm(),
      sector: this.selectedSector()
    });
  }

  onSearchChange(term: string): void {
    this.searchTerm.set(term);
    this.triggerFetch();
  }

  onClearSearch(): void {
    this.searchTerm.set('');
    this.triggerFetch();
  }

  toggleSector(sector: string): void {
    if (this.selectedSector() === sector) {
      this.selectedSector.set(null);
    } else {
      this.selectedSector.set(sector);
    }
    this.triggerFetch();
  }

  onCompanyClick(companyId: string): void {
    this.router.navigate(['/company', companyId]);
  }

  retry(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.apiService.getCompanies(
      this.searchTerm().trim() || undefined,
      this.selectedSector() || undefined
    ).subscribe({
      next: (data) => {
        this.companies.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.isLoading.set(false);
        this.errorMessage.set(err.message || 'Failed to load companies.');
      }
    });
  }
}
