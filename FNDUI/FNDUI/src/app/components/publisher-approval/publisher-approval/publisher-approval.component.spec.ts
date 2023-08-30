import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublisherApprovalComponent } from './publisher-approval.component';

describe('PublisherApprovalComponent', () => {
  let component: PublisherApprovalComponent;
  let fixture: ComponentFixture<PublisherApprovalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PublisherApprovalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PublisherApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
