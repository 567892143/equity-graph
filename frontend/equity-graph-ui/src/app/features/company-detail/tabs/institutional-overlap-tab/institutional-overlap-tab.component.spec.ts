import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InstitutionalOverlapTabComponent } from './institutional-overlap-tab.component';

describe('InstitutionalOverlapTabComponent', () => {
  let component: InstitutionalOverlapTabComponent;
  let fixture: ComponentFixture<InstitutionalOverlapTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InstitutionalOverlapTabComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InstitutionalOverlapTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
