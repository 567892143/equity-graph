import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BoardInterlocksTabComponent } from './board-interlocks-tab.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';

describe('BoardInterlocksTabComponent', () => {
  let component: BoardInterlocksTabComponent;
  let fixture: ComponentFixture<BoardInterlocksTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BoardInterlocksTabComponent],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([])
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BoardInterlocksTabComponent);
    component = fixture.componentInstance;
    component.companyId = 'comp-1';
    component.companyName = 'Tata Consultancy Services';
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
