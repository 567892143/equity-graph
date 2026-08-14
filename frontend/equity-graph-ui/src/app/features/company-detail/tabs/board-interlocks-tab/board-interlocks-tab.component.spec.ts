import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoardInterlocksTabComponent } from './board-interlocks-tab.component';

describe('BoardInterlocksTabComponent', () => {
  let component: BoardInterlocksTabComponent;
  let fixture: ComponentFixture<BoardInterlocksTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BoardInterlocksTabComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BoardInterlocksTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
