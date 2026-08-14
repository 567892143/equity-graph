import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'marketCapFormat',
  standalone: true
})
export class MarketCapFormatPipe implements PipeTransform {
  transform(value: number | null | undefined): string {
    if (value === null || value === undefined || isNaN(value)) {
      return '—';
    }

    if (value === 0) {
      return '₹0 Cr';
    }

    // Convert raw rupees to Crores (1 Crore = 10^7 = 10,000,000)
    // If value is already in crore scale (< 10^7), format directly, otherwise divide by 10^7
    const inCrores = value >= 10000000 ? value / 10000000 : value;

    // Format with Indian numbering system (e.g., 1,45,000 or 16,000)
    const formatted = Math.round(inCrores).toLocaleString('en-IN');
    return `₹${formatted} Cr`;
  }
}
