import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-error-state',
  imports: [CommonModule],
  templateUrl: './error-state.component.html',
  styleUrl: './error-state.component.scss'
})
export class ErrorStateComponent {
  @Input() message: string = 'An error occurred while loading data.';
  @Input() showRetry: boolean = true;
  @Output() retry = new EventEmitter<void>();

  onRetryClick(): void {
    this.retry.emit();
  }
}
