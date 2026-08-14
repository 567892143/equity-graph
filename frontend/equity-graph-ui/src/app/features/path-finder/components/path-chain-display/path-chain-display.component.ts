import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { PathNode } from '../../../../core/models/shortest-path';

@Component({
  selector: 'app-path-chain-display',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './path-chain-display.component.html',
  styleUrl: './path-chain-display.component.scss'
})
export class PathChainDisplayComponent {
  private readonly router = inject(Router);

  @Input({ required: true }) nodes: PathNode[] = [];
  @Input({ required: true }) relationshipTypes: string[] = [];

  getNodeBadgeClass(label: string): string {
    const norm = (label || '').toLowerCase();
    if (norm.includes('company')) return 'badge-company';
    if (norm.includes('person') || norm.includes('director')) return 'badge-person';
    if (norm.includes('institution')) return 'badge-institution';
    return 'badge-default';
  }

  formatRelationship(type: string): string {
    if (!type) return '';
    return type.replace(/_/g, ' ');
  }

  onNodeClick(node: PathNode): void {
    // If the node is a company, allow drilling down to its detail page
    if (node.label?.toLowerCase() === 'company') {
      this.router.navigate(['/company', node.id]);
    }
  }
}
