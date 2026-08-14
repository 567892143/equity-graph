import { Component, Input, OnChanges, SimpleChanges, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { EquityGraphApiService } from '../../../../core/services/equity-graph-api.service';
import { BoardInterlock } from '../../../../core/models/board-interlock';
import {
  GraphVisualizationComponent,
  GraphCenterNode,
  GraphConnection,
  GraphLegendItem
} from '../../../../shared/components/graph-visualization/graph-visualization.component';
import { LoadingSkeletonComponent } from '../../../../shared/components/loading-skeleton/loading-skeleton.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { ErrorStateComponent } from '../../../../shared/components/error-state/error-state.component';

@Component({
  selector: 'app-board-interlocks-tab',
  standalone: true,
  imports: [
    CommonModule,
    GraphVisualizationComponent,
    LoadingSkeletonComponent,
    EmptyStateComponent,
    ErrorStateComponent
  ],
  templateUrl: './board-interlocks-tab.component.html',
  styleUrl: './board-interlocks-tab.component.scss'
})
export class BoardInterlocksTabComponent implements OnChanges {
  private readonly apiService = inject(EquityGraphApiService);
  private readonly router = inject(Router);

  @Input({ required: true }) companyId!: string;
  @Input({ required: true }) companyName!: string;
  @Input() active: boolean = false;

  readonly interlocks = signal<BoardInterlock[]>([]);
  readonly isLoading = signal<boolean>(false);
  readonly errorMessage = signal<string | null>(null);

  // Cache flag to prevent duplicate network calls on tab toggle
  private isLoaded = false;
  private loadedCompanyId: string | null = null;

  readonly graphLegend: GraphLegendItem[] = [
    { label: 'DIRECTOR_OF', color: '#3B82F6' }
  ];

  get centerNode(): GraphCenterNode {
    return {
      id: this.companyId,
      name: this.companyName,
      label: 'Focus Company'
    };
  }

  get graphConnections(): GraphConnection[] {
    return this.interlocks().map((item) => ({
      id: item.otherCompanyId,
      name: `${item.personName} (${item.otherCompanyName})`,
      label: item.personName,
      relationshipType: 'DIRECTOR_OF'
    }));
  }

  ngOnChanges(changes: SimpleChanges): void {
    // If company changed, invalidate cache
    if (changes['companyId'] && changes['companyId'].currentValue !== this.loadedCompanyId) {
      this.isLoaded = false;
      this.loadedCompanyId = null;
    }

    // Lazy load on first activation
    if (this.active && !this.isLoaded && this.companyId) {
      this.fetchInterlocks();
    }
  }

  fetchInterlocks(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.apiService.getBoardInterlocks(this.companyId).subscribe({
      next: (data) => {
        this.interlocks.set(data);
        this.isLoading.set(false);
        this.isLoaded = true;
        this.loadedCompanyId = this.companyId;
      },
      error: (err) => {
        this.isLoading.set(false);
        this.errorMessage.set(err.message || 'Failed to load board interlocks.');
      }
    });
  }

  onNavigateCompany(otherCompanyId: string): void {
    this.router.navigate(['/company', otherCompanyId]);
  }
}
