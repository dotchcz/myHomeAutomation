import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PhotovoltaicsComponent } from './photovoltaics.component';

describe('PhotovoltaicsComponent', () => {
  let component: PhotovoltaicsComponent;
  let fixture: ComponentFixture<PhotovoltaicsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PhotovoltaicsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PhotovoltaicsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
