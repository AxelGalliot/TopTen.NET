import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoundReadyComponent } from './round-ready.component';

describe('RoundReadyComponent', () => {
  let component: RoundReadyComponent;
  let fixture: ComponentFixture<RoundReadyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RoundReadyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RoundReadyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
