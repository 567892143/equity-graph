import { Component, OnInit, OnDestroy, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { EquityGraphApiService } from '../../core/services/equity-graph-api.service';
import { CompanySummary } from '../../core/models/company';
import { ShortestPathResponse } from '../../core/models/shortest-path';
import { ApiError } from '../../core/interceptors/error.interceptor';
import { PathChainDisplayComponent } from './components/path-chain-display/path-chain-display.component';
import { LoadingSkeletonComponent } from '../../shared/components/loading-skeleton/loading-skeleton.component';
import { EmptyStateComponent } from '../../shared/components/empty-state/empty-state.component';
import { ErrorStateComponent } from '../../shared/components/error-state/error-state.component';

@Component({
  selector: 'app-path-finder',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    PathChainDisplayComponent,
    LoadingSkeletonComponent,
    EmptyStateComponent,
    ErrorStateComponent
  ],
  templateUrl: './path-finder.component.html',
  styleUrl: './path-finder.component.scss'
})
export class PathFinderComponent implements OnInit, OnDestroy {
  private readonly apiService = inject(EquityGraphApiService);

  // Path Finder State Signals
  readonly fromCompany = signal<CompanySummary | null>(null);
  readonly toCompany = signal<CompanySummary | null>(null);
  readonly pathResult = signal<ShortestPathResponse | null>(null);
  readonly isLoading = signal<boolean>(false);
  readonly errorMessage = signal<string | null>(null);
  readonly noPathFound = signal<boolean>(false);

  // From Company Autocomplete State
  readonly fromSearchTerm = signal<string>('');
  readonly fromSearchResults = signal<CompanySummary[]>([]);
  readonly isFromDropdownOpen = signal<boolean>(false);

  // To Company Autocomplete State
  readonly toSearchTerm = signal<string>('');
  readonly toSearchResults = signal<CompanySummary[]>([]);
  readonly isToDropdownOpen = signal<boolean>(false);

  // Search subjects
  private readonly fromSearchSubject$ = new Subject<string>();
  private readonly toSearchSubject$ = new Subject<string>();
  private fromSearchSub?: Subscription;
  private toSearchSub?: Subscription;

  ngOnInit(): void {
    this.setupAutocompleteStreams();
  }

  ngOnDestroy(): void {
    this.fromSearchSub?.unsubscribe();
    this.toSearchSub?.unsubscribe();
  }

  private setupAutocompleteStreams(): void {
    this.fromSearchSub = this.fromSearchSubject$
      .pipe(
        debounceTime(250),
        distinctUntilChanged(),
        switchMap((term) => this.apiService.getCompanies(term.trim() || undefined))
      )
      .subscribe({
        next: (res) => this.fromSearchResults.set(res),
        error: () => this.fromSearchResults.set([])
      });

    this.toSearchSub = this.toSearchSubject$
      .pipe(
        debounceTime(250),
        distinctUntilChanged(),
        switchMap((term) => this.apiService.getCompanies(term.trim() || undefined))
      )
      .subscribe({
        next: (res) => this.toSearchResults.set(res),
        error: () => this.toSearchResults.set([])
      });
  }

  onFromSearchInput(term: string): void {
    this.fromSearchTerm.set(term);
    this.isFromDropdownOpen.set(true);
    this.fromSearchSubject$.next(term);
  }

  onToSearchInput(term: string): void {
    this.toSearchTerm.set(term);
    this.isToDropdownOpen.set(true);
    this.toSearchSubject$.next(term);
  }

  selectFromCompany(company: CompanySummary): void {
    this.fromCompany.set(company);
    this.fromSearchTerm.set(company.name);
    this.isFromDropdownOpen.set(false);
  }

  selectToCompany(company: CompanySummary): void {
    this.toCompany.set(company);
    this.toSearchTerm.set(company.name);
    this.isToDropdownOpen.set(false);
  }

  clearFromCompany(): void {
    this.fromCompany.set(null);
    this.fromSearchTerm.set('');
    this.pathResult.set(null);
    this.noPathFound.set(false);
  }

  clearToCompany(): void {
    this.toCompany.set(null);
    this.toSearchTerm.set('');
    this.pathResult.set(null);
    this.noPathFound.set(false);
  }

  swapCompanies(): void {
    const tempCompany = this.fromCompany();
    const tempTerm = this.fromSearchTerm();

    this.fromCompany.set(this.toCompany());
    this.fromSearchTerm.set(this.toSearchTerm());

    this.toCompany.set(tempCompany);
    this.toSearchTerm.set(tempTerm);

    if (this.fromCompany() && this.toCompany()) {
      this.findConnection();
    }
  }

  findConnection(): void {
    const from = this.fromCompany();
    const to = this.toCompany();

    if (!from || !to) return;

    if (from.id === to.id) {
      this.errorMessage.set('Please select two distinct companies to find a connection path.');
      this.pathResult.set(null);
      this.noPathFound.set(false);
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.noPathFound.set(false);
    this.pathResult.set(null);

    this.apiService.getShortestPath(from.id, to.id).subscribe({
      next: (data) => {
        this.pathResult.set(data);
        this.isLoading.set(false);
        this.noPathFound.set(false);
        this.errorMessage.set(null);
      },
      error: (err: ApiError) => {
        this.isLoading.set(false);
        // 404 indicates "No connection found", which is an expected outcome (empty state), not a failure
        if (err.status === 404) {
          this.noPathFound.set(true);
          this.pathResult.set(null);
          this.errorMessage.set(null);
        } else {
          this.noPathFound.set(false);
          this.pathResult.set(null);
          this.errorMessage.set(err.message || 'An unexpected error occurred while querying the network graph.');
        }
      }
    });
  }
}
