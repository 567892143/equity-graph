import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CompanySummary } from '../../../../core/models/company';
import { MarketCapFormatPipe } from '../../../../shared/pipes/market-cap-format.pipe';

@Component({
  selector: 'app-company-card',
  standalone: true,
  imports: [CommonModule, MarketCapFormatPipe],
  templateUrl: './company-card.component.html',
  styleUrl: './company-card.component.scss'
})
export class CompanyCardComponent {
  @Input({ required: true }) company!: CompanySummary;
  @Output() cardClick = new EventEmitter<string>();

  onCardClick(): void {
    if (this.company?.id) {
      this.cardClick.emit(this.company.id);
    }
  }
}
