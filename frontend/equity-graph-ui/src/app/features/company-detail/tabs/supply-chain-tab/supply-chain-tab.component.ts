import { Component, Input, OnChanges, SimpleChanges, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { EquityGraphApiService } from '../../../../core/services/equity-graph-api.service';
import { SupplyChainPath } from '../../../../core/models/supply-chain-path';
import { LoadingSkeletonComponent } from '../../../../shared/components/loading-skeleton/loading-skeleton.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { ErrorStateComponent } from '../../../../shared/components/error-state/error-state.component';

@Component({
  selector: 'app-supply-chain-tab',
  standalone: true,
  imports: [
    CommonModule,
    LoadingSkeletonComponent,
    EmptyStateComponent,
    ErrorStateComponent
  ],
  templateUrl: './supply-chain-tab.component.html',
  styleUrl: './supply-chain-tab.component.scss'
})
export class SupplyChainTabComponent implements OnChanges {
  private readonly apiService = inject(EquityGraphApiService);
  private readonly router = inject(Router);

  @Input({ required: true }) companyId!: string;
  @Input({ required: true }) companyName!: string;
  @Input() active: boolean = false;

  readonly selectedHops = signal<number>(2);
  readonly paths = signal<SupplyChainPath[]>([]);
  readonly isLoading = signal<boolean>(false);
  readonly errorMessage = signal<string | null>(null);

  // Cache per hop depth to prevent re-fetching visited depths
  private cache = new Map<number, SupplyChainPath[]>();
  private loadedCompanyId: string | null = null;

  readonly availableHops = [1, 2, 3];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['companyId'] && changes['companyId'].currentValue !== this.loadedCompanyId) {
      this.cache.clear();
      this.loadedCompanyId = this.companyId;
    }

    if (this.active && this.companyId) {
      this.loadExposure(this.selectedHops());
    }
  }

  setHopDepth(hops: number): void {
    if (this.selectedHops() === hops) return;
    this.selectedHops.set(hops);
    this.loadExposure(hops);
  }

  loadExposure(hops: number): void {
    // Check cache
    if (this.cache.has(hops)) {
      this.paths.set(this.cache.get(hops)!);
      this.isLoading.set(false);
      this.errorMessage.set(null);
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.apiService.getSupplyChainExposure(this.companyId, hops).subscribe({
      next: (data) => {
        this.cache.set(hops, data);
        this.paths.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.isLoading.set(false);
        this.errorMessage.set(err.message || 'Failed to load supply chain exposure data.');
      }
    });
  }

  onNavigateCompany(nodeId: string): void {
    if (nodeId !== this.companyId) {
      this.router.navigate(['/company', nodeId]);
    }
  }
}
