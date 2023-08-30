import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InviteModeratorModalComponent } from './invite-moderator-modal.component';

describe('InviteModeratorComponent', () => {
  let component: InviteModeratorModalComponent;
  let fixture: ComponentFixture<InviteModeratorModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InviteModeratorModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InviteModeratorModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
