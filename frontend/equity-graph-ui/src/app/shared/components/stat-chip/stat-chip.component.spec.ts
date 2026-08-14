import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatChipComponent } from './stat-chip.component';

describe('StatChipComponent', () => {
  let component: StatChipComponent;
  let fixture: ComponentFixture<StatChipComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StatChipComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StatChipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
