import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupplyChainTabComponent } from './supply-chain-tab.component';

describe('SupplyChainTabComponent', () => {
  let component: SupplyChainTabComponent;
  let fixture: ComponentFixture<SupplyChainTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SupplyChainTabComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SupplyChainTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
