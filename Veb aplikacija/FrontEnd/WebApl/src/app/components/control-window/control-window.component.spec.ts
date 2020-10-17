import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlWindowComponent } from './control-window.component';

describe('ControlWindowComponent', () => {
  let component: ControlWindowComponent;
  let fixture: ComponentFixture<ControlWindowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlWindowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlWindowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
