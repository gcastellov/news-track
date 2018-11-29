import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FailureComponent } from './failure.component';

describe('FailureComponent', () => {
  let component: FailureComponent;
  let fixture: ComponentFixture<FailureComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FailureComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FailureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
