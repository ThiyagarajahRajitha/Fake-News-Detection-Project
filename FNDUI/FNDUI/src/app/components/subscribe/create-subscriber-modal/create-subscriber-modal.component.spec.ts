import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSubscriberModalComponent } from './create-subscriber-modal.component';

describe('CreateSubscriberModalComponent', () => {
  let component: CreateSubscriberModalComponent;
  let fixture: ComponentFixture<CreateSubscriberModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateSubscriberModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateSubscriberModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
