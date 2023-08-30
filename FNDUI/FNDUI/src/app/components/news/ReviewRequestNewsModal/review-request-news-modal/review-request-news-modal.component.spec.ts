import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewRequestNewsModalComponent } from './review-request-news-modal.component';

describe('ReviewRequestNewsModalComponent', () => {
  let component: ReviewRequestNewsModalComponent;
  let fixture: ComponentFixture<ReviewRequestNewsModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReviewRequestNewsModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReviewRequestNewsModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
