import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SupplyChainTabComponent } from './supply-chain-tab.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';

describe('SupplyChainTabComponent', () => {
  let component: SupplyChainTabComponent;
  let fixture: ComponentFixture<SupplyChainTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SupplyChainTabComponent],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([])
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SupplyChainTabComponent);
    component = fixture.componentInstance;
    component.companyId = 'comp-1';
    component.companyName = 'Tata Consultancy Services';
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
