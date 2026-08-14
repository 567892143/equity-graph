import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CompanyCardComponent } from './company-card.component';

describe('CompanyCardComponent', () => {
  let component: CompanyCardComponent;
  let fixture: ComponentFixture<CompanyCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CompanyCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CompanyCardComponent);
    component = fixture.componentInstance;
    component.company = {
      id: 'comp-1',
      name: 'Tata Consultancy Services',
      ticker: 'TCS.NS',
      sector: 'Information Technology',
      marketCap: 160000000000
    };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should emit cardClick when clicked', () => {
    spyOn(component.cardClick, 'emit');
    component.onCardClick();
    expect(component.cardClick.emit).toHaveBeenCalledWith('comp-1');
  });
});
