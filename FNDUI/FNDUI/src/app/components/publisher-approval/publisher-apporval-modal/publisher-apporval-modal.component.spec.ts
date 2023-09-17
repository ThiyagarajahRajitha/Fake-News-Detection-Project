import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublisherApporvalModalComponent } from './publisher-apporval-modal.component';

describe('PublisherApporvalModalComponent', () => {
  let component: PublisherApporvalModalComponent;
  let fixture: ComponentFixture<PublisherApporvalModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PublisherApporvalModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PublisherApporvalModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
