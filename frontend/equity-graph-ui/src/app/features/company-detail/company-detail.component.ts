import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { EquityGraphApiService } from '../../core/services/equity-graph-api.service';
import { CompanyDetailResponse } from '../../core/models/company';
import { ApiError } from '../../core/interceptors/error.interceptor';
import { MarketCapFormatPipe } from '../../shared/pipes/market-cap-format.pipe';
import { StatChipComponent } from '../../shared/components/stat-chip/stat-chip.component';
import { LoadingSkeletonComponent } from '../../shared/components/loading-skeleton/loading-skeleton.component';
import { ErrorStateComponent } from '../../shared/components/error-state/error-state.component';
import { BoardInterlocksTabComponent } from './tabs/board-interlocks-tab/board-interlocks-tab.component';
import { SupplyChainTabComponent } from './tabs/supply-chain-tab/supply-chain-tab.component';
import { InstitutionalOverlapTabComponent } from './tabs/institutional-overlap-tab/institutional-overlap-tab.component';

export type DetailTab = 'interlocks' | 'supply' | 'institutional';

@Component({
  selector: 'app-company-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MarketCapFormatPipe,
    StatChipComponent,
    LoadingSkeletonComponent,
    ErrorStateComponent,
    BoardInterlocksTabComponent,
    SupplyChainTabComponent,
    InstitutionalOverlapTabComponent
  ],
  templateUrl: './company-detail.component.html',
  styleUrl: './company-detail.component.scss'
})
export class CompanyDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly apiService = inject(EquityGraphApiService);

  // Screen State Signals
  readonly companyId = signal<string>('');
  readonly company = signal<CompanyDetailResponse | null>(null);
  readonly isLoading = signal<boolean>(true);
  readonly errorMessage = signal<string | null>(null);
  readonly isNotFound = signal<boolean>(false);
  readonly activeTab = signal<DetailTab>('interlocks');

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) {
        this.companyId.set(id);
        this.loadCompanyDetails(id);
      }
    });
  }

  loadCompanyDetails(id: string): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.isNotFound.set(false);

    this.apiService.getCompanyById(id).subscribe({
      next: (data) => {
        this.company.set(data);
        this.isLoading.set(false);
        this.isNotFound.set(false);
      },
      error: (err: ApiError) => {
        this.isLoading.set(false);
        if (err.status === 404) {
          this.isNotFound.set(true);
          this.errorMessage.set(`Company with ID '${id}' was not found.`);
        } else {
          this.isNotFound.set(false);
          this.errorMessage.set(err.message || 'Failed to load company details.');
        }
      }
    });
  }

  setActiveTab(tab: DetailTab): void {
    this.activeTab.set(tab);
  }

  navigateHome(): void {
    this.router.navigate(['/']);
  }

  /**
   * Risk Severity Evaluation Thresholds:
   * 1. Board Interlocks:
   *    - 0 interlocks: 'low' (independent board governance)
   *    - 1-2 interlocks: 'medium' (moderate network overlap)
   *    - 3+ interlocks: 'high' (significant systemic/governance concentration)
   */
  getBoardInterlockSeverity(count: number): 'low' | 'medium' | 'high' | 'neutral' {
    if (count === 0) return 'low';
    if (count <= 2) return 'medium';
    return 'high';
  }

  /**
   * 2. Max Supply Chain Dependency:
   *    - < 15%: 'low' (diversified supply base)
   *    - 15% - 30%: 'medium' (moderate single-supplier reliance)
   *    - > 30%: 'high' (critical single-point-of-failure risk)
   */
  getSupplyDependencySeverity(pct: number): 'low' | 'medium' | 'high' | 'neutral' {
    if (pct < 15) return 'low';
    if (pct <= 30) return 'medium';
    return 'high';
  }

  /**
   * 3. Institutional Ownership Concentration:
   *    - 0: 'neutral'
   *    - 1-5: 'low'
   *    - > 5: 'neutral' (standard institutional float)
   */
  getInstitutionCountSeverity(count: number): 'low' | 'medium' | 'high' | 'neutral' {
    if (count === 0) return 'neutral';
    if (count <= 5) return 'low';
    return 'neutral';
  }
}
