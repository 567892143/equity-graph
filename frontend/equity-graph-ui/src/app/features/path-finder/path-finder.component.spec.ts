import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PathFinderComponent } from './path-finder.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';

describe('PathFinderComponent', () => {
  let component: PathFinderComponent;
  let fixture: ComponentFixture<PathFinderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PathFinderComponent],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([])
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PathFinderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
