import { TestBed } from '@angular/core/testing';

import { EquityGraphApiService } from './equity-graph-api.service';

describe('EquityGraphApiService', () => {
  let service: EquityGraphApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EquityGraphApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
