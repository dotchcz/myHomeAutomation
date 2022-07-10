import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RelayDetailComponent } from './relay-detail.component';

describe('RelayDetailComponent', () => {
  let component: RelayDetailComponent;
  let fixture: ComponentFixture<RelayDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RelayDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RelayDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
