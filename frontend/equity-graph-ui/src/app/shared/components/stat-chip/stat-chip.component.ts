import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-stat-chip',
  imports: [CommonModule],
  templateUrl: './stat-chip.component.html',
  styleUrl: './stat-chip.component.scss'
})
export class StatChipComponent {
  @Input() label: string = '';
  @Input() value: string | number = '';
  @Input() severity: 'low' | 'medium' | 'high' | 'neutral' = 'neutral';
  @Input() clickable: boolean = false;
  @Output() chipClick = new EventEmitter<void>();

  onClick(): void {
    if (this.clickable) {
      this.chipClick.emit();
    }
  }
}
