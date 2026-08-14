import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { InfoModalComponent } from './shared/components/info-modal/info-modal.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    InfoModalComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'EquityGraph';
  readonly isInfoModalOpen = signal<boolean>(false);

  openInfoModal(): void {
    this.isInfoModalOpen.set(true);
  }

  closeInfoModal(): void {
    this.isInfoModalOpen.set(false);
  }
}
