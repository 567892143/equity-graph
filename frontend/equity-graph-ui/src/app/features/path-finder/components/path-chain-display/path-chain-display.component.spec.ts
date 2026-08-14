import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PathChainDisplayComponent } from './path-chain-display.component';

describe('PathChainDisplayComponent', () => {
  let component: PathChainDisplayComponent;
  let fixture: ComponentFixture<PathChainDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PathChainDisplayComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PathChainDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
