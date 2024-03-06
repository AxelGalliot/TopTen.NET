import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CTAComponent } from './cta.component';

describe('CTAComponent', () => {
  let component: CTAComponent;
  let fixture: ComponentFixture<CTAComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CTAComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CTAComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
