import { ComponentFixture, TestBed } from '@angular/core/testing';
import { GraphVisualizationComponent } from './graph-visualization.component';

describe('GraphVisualizationComponent', () => {
  let component: GraphVisualizationComponent;
  let fixture: ComponentFixture<GraphVisualizationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GraphVisualizationComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(GraphVisualizationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should layout 1 node directly at top center', () => {
    component.centerNode = { id: 'c1', name: 'Center', label: 'Company' };
    component.connections = [
      { id: 'n1', name: 'Node 1', label: 'Person', relationshipType: 'DIRECTOR_OF' }
    ];
    component.computeLayout();

    expect(component.renderedNodes.length).toBe(1);
    expect(component.renderedNodes[0].x).toBeCloseTo(320, 1);
    expect(component.renderedNodes[0].y).toBeCloseTo(250 - 160, 1); // y = 90
  });

  it('should layout 2 nodes in an upward V-spread rather than vertical straight line', () => {
    component.centerNode = { id: 'c1', name: 'Center', label: 'Company' };
    component.connections = [
      { id: 'n1', name: 'Node 1', label: 'Person', relationshipType: 'DIRECTOR_OF' },
      { id: 'n2', name: 'Node 2', label: 'Person', relationshipType: 'DIRECTOR_OF' }
    ];
    component.computeLayout();

    expect(component.renderedNodes.length).toBe(2);
    // Node 0 should be at -135° (top-left)
    expect(component.renderedNodes[0].x).toBeLessThan(320);
    expect(component.renderedNodes[0].y).toBeLessThan(250);

    // Node 1 should be at -45° (top-right)
    expect(component.renderedNodes[1].x).toBeGreaterThan(320);
    expect(component.renderedNodes[1].y).toBeLessThan(250);

    // Both should share the same Y level in the upper quadrant
    expect(component.renderedNodes[0].y).toBeCloseTo(component.renderedNodes[1].y, 1);
  });

  it('should not truncate names under 20 characters', () => {
    expect(component.truncateName('Ireena Vittal', 20)).toBe('Ireena Vittal');
    expect(component.truncateName('N. Chandrasekaran', 20)).toBe('N. Chandrasekaran');
    expect(component.truncateName('A very long corporation name that exceeds twenty characters', 20)).toBe(
      'A very long corpora…'
    );
  });
});
