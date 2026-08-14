import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PathChainDisplayComponent } from './path-chain-display.component';
import { provideRouter } from '@angular/router';

describe('PathChainDisplayComponent', () => {
  let component: PathChainDisplayComponent;
  let fixture: ComponentFixture<PathChainDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PathChainDisplayComponent],
      providers: [provideRouter([])]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PathChainDisplayComponent);
    component = fixture.componentInstance;
    component.nodes = [
      { id: 'comp-1', name: 'Tata Consultancy Services', label: 'Company' },
      { id: 'person-1', name: 'Natarajan Chandrasekaran', label: 'Person' },
      { id: 'comp-3', name: 'Tata Motors Limited', label: 'Company' }
    ];
    component.relationshipTypes = ['DIRECTOR_OF', 'DIRECTOR_OF'];
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
