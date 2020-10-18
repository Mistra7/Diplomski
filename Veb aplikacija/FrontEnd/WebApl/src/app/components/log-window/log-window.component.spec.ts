import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LogWindowComponent } from './log-window.component';

describe('LogWindowComponent', () => {
  let component: LogWindowComponent;
  let fixture: ComponentFixture<LogWindowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LogWindowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LogWindowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
