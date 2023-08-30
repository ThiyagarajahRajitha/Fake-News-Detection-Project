import { TestBed } from '@angular/core/testing';

import { PublisherApprovalService } from './publisher-approval.service';

describe('PublisherApprovalService', () => {
  let service: PublisherApprovalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PublisherApprovalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
