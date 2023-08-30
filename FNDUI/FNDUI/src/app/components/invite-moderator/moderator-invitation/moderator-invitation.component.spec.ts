import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModeratorInvitationComponent } from './moderator-invitation.component';

describe('ModeratorInvitationComponent', () => {
  let component: ModeratorInvitationComponent;
  let fixture: ComponentFixture<ModeratorInvitationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModeratorInvitationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModeratorInvitationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
