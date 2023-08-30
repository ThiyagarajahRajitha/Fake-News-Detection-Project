import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TabbedViewComponent } from './tabbed-view.component';

describe('TabbedViewComponent', () => {
  let component: TabbedViewComponent;
  let fixture: ComponentFixture<TabbedViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TabbedViewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TabbedViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
