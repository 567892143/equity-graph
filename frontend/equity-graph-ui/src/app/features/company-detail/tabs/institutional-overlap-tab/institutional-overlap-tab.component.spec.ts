import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InstitutionalOverlapTabComponent } from './institutional-overlap-tab.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('InstitutionalOverlapTabComponent', () => {
  let component: InstitutionalOverlapTabComponent;
  let fixture: ComponentFixture<InstitutionalOverlapTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InstitutionalOverlapTabComponent],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InstitutionalOverlapTabComponent);
    component = fixture.componentInstance;
    component.companyId = 'comp-1';
    component.companyName = 'Tata Consultancy Services';
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
