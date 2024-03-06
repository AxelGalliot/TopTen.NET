import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvatarpickerComponent } from './avatarpicker.component';

describe('AvatarpickerComponent', () => {
  let component: AvatarpickerComponent;
  let fixture: ComponentFixture<AvatarpickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AvatarpickerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AvatarpickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
