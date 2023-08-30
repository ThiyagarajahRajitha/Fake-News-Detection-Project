import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassifyNewsModalComponent } from './classify-news-modal.component';

describe('ClassifyNewsModalComponent', () => {
  let component: ClassifyNewsModalComponent;
  let fixture: ComponentFixture<ClassifyNewsModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClassifyNewsModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClassifyNewsModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
