import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewRequestedNewsComponent } from './review-requested-news.component';

describe('ReviewRequestedNewsComponent', () => {
  let component: ReviewRequestedNewsComponent;
  let fixture: ComponentFixture<ReviewRequestedNewsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReviewRequestedNewsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReviewRequestedNewsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
