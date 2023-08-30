import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModeratorSignupComponent } from './moderator-signup.component';

describe('ModeratorSignupComponent', () => {
  let component: ModeratorSignupComponent;
  let fixture: ComponentFixture<ModeratorSignupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModeratorSignupComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModeratorSignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
