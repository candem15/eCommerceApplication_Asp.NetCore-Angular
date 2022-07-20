import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectProductImagesDialogComponent } from './select-product-images-dialog.component';

describe('SelectProductImagesDialogComponent', () => {
  let component: SelectProductImagesDialogComponent;
  let fixture: ComponentFixture<SelectProductImagesDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SelectProductImagesDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectProductImagesDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
