import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface GraphCenterNode {
  id: string;
  name: string;
  label: string;
}

export interface GraphConnection {
  id: string;
  name: string;
  label: string;
  relationshipType: string;
}

export interface GraphLegendItem {
  label: string;
  color: string;
}

interface RenderedNode {
  id: string;
  name: string;
  label: string;
  relationshipType: string;
  x: number;
  y: number;
  color: string;
  shortName: string;
}

interface RenderedEdge {
  id: string;
  x1: number;
  y1: number;
  x2: number;
  y2: number;
  color: string;
  relationshipType: string;
}

@Component({
  selector: 'app-graph-visualization',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './graph-visualization.component.html',
  styleUrl: './graph-visualization.component.scss'
})
export class GraphVisualizationComponent implements OnChanges {
  @Input() centerNode: GraphCenterNode | null = null;
  @Input() connections: GraphConnection[] = [];
  @Input() legend: GraphLegendItem[] = [];

  readonly width = 640;
  readonly height = 500;
  readonly cx = 320;
  readonly cy = 250;
  readonly radius = 160;

  renderedNodes: RenderedNode[] = [];
  renderedEdges: RenderedEdge[] = [];
  hoveredNodeId: string | null = null;

  ngOnChanges(changes: SimpleChanges): void {
    this.computeLayout();
  }

  computeLayout(): void {
    if (!this.connections || this.connections.length === 0) {
      this.renderedNodes = [];
      this.renderedEdges = [];
      return;
    }

    const count = this.connections.length;

    this.renderedNodes = this.connections.map((conn, index) => {
      let angle: number;

      // Special branch for low connection counts to avoid degenerate straight lines
      if (count === 1) {
        // Single connection: position at top center (-90° / -PI/2)
        angle = -Math.PI / 2;
      } else if (count === 2) {
        // Two connections: spread asymmetrically in an upward V-shape (-135° and -45°)
        // instead of 180° opposite straight vertical line
        angle = index === 0 ? -Math.PI / 2 - Math.PI / 4 : -Math.PI / 2 + Math.PI / 4;
      } else {
        // 3+ connections: standard uniform circular distribution
        const angleStep = (2 * Math.PI) / count;
        angle = index * angleStep - Math.PI / 2;
      }

      const x = this.cx + this.radius * Math.cos(angle);
      const y = this.cy + this.radius * Math.sin(angle);
      const color = this.getColorForRelationship(conn.relationshipType);

      return {
        id: conn.id,
        name: conn.name,
        label: conn.label,
        relationshipType: conn.relationshipType,
        x,
        y,
        color,
        shortName: this.truncateName(conn.name, 20)
      };
    });

    this.renderedEdges = this.renderedNodes.map((node) => ({
      id: `edge-${node.id}`,
      x1: this.cx,
      y1: this.cy,
      x2: node.x,
      y2: node.y,
      color: node.color,
      relationshipType: node.relationshipType
    }));
  }

  getColorForRelationship(relationshipType: string): string {
    const matched = this.legend?.find(
      (item) => item.label.toLowerCase() === relationshipType?.toLowerCase()
    );
    if (matched) {
      return matched.color;
    }

    // Default fallback color mapping
    const normalized = relationshipType?.toUpperCase();
    if (normalized?.includes('DIRECTOR')) return '#3B82F6'; // Blue
    if (normalized?.includes('SUPPL')) return '#F59E0B';   // Amber
    if (normalized?.includes('STAKE') || normalized?.includes('INSTITUTION')) return '#10B981'; // Green
    if (normalized?.includes('OWN')) return '#8B5CF6';     // Purple
    return '#94A3B8'; // Slate
  }

  truncateName(name: string, maxLength: number = 20): string {
    if (!name) return '';
    return name.length > maxLength ? name.substring(0, maxLength - 1) + '…' : name;
  }

  onNodeHover(nodeId: string | null): void {
    this.hoveredNodeId = nodeId;
  }
}
