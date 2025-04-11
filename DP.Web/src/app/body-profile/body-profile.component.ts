import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { GeneralState } from '../stores/store.state';
import * as GeneralActions from '../stores/store.actions';
import * as BodyProfileActions from './stores/body-profile.actions';
import * as GeneralSelector from '../stores/store.selectors';
import * as BodyProfileSelector from './stores/body-profile.selectors';
import { Router } from '@angular/router';
import { UntypedFormBuilder, Validators } from '@angular/forms';
import { UserProfile } from './models/user-profile';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ImageCroppedEvent, LoadedImage } from 'ngx-image-cropper';

@UntilDestroy()
@Component({
    selector: 'app-body-profile',
    templateUrl: './body-profile.component.html',
    styleUrls: ['./body-profile.component.css'],
    standalone: false
})
export class BodyProfileComponent implements OnInit {
  public measurements$ = this.store.select(BodyProfileSelector.getMeasurements);
  public userProfile$ = this.store.select(BodyProfileSelector.getUserProfile);
  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);

  public imageChangedEvent: any;
  public croppedImage: string;
  public inAvatarEditMode: boolean = false;

  private processingMeasurementId: number;

  constructor(private store: Store<GeneralState>,
    private router: Router,
    private formBuilder: UntypedFormBuilder) { }

  public userProfileForm = this.formBuilder.group({
    name: ['', [Validators.required, Validators.maxLength(64)]],
    gender: [''],
    birthDate: [''],
    height: ['', [Validators.maxLength(3), Validators.pattern('^[0-9]*$')]],
  });

  ngOnInit(): void {
    this.store.dispatch(GeneralActions.clearErrors());
    this.store.dispatch(BodyProfileActions.getMeasurementsRequest());
    this.store.dispatch(BodyProfileActions.getUserProfileRequest());

    this.userProfile$.pipe(untilDestroyed(this)).subscribe(userProfile => {
      if (userProfile) {
        this.userProfileForm.get('name')?.setValue(userProfile.name);
        this.userProfileForm.get('gender')?.setValue(userProfile.gender);
        this.userProfileForm.get('birthDate')?.setValue(userProfile.birthDate.split('T')[0]);
        this.userProfileForm.get('height')?.setValue(userProfile.height);
      }
    });
  }

  onEditButtonClick($event: any): void {
    this.router.navigate(['body-profile/edit/' + ($event.target.parentElement as HTMLInputElement).value]);
  }

  onRemoveButtonClick($event: any): void {
    this.processingMeasurementId = Number(($event.target.parentElement as HTMLInputElement).value);
  }

  removeConfirmationButtonClick(): void {
    if (!this.processingMeasurementId) {
      return;
    }
    this.store.dispatch(BodyProfileActions.removeMeasurementRequest({ measurementId: this.processingMeasurementId }));
  }

  public onSubmitUserProfile(): void {
    if (!this.userProfileForm.valid) {
      this.userProfileForm.markAllAsTouched();
      return;
    }

    this.store.dispatch(
      BodyProfileActions.updateUserProfileRequest({
        userProfile: {
          name: this.getControlValue('name'),
          gender: this.getControlValue('gender'),
          birthDate: this.getControlValue('birthDate'),
          height: this.getControlValue('height'),
        } as UserProfile,
      }),
    );
  }

  public openFileSelection() {
    const fileInput: HTMLElement = document.querySelector('input[type="file"]');
    if (fileInput) {
      fileInput.click();
    }
  }

  public onFileSelected(event) {
    const file = event.target.files[0];

    let reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => this.store.dispatch(BodyProfileActions.uploadUserAvatarRequest({ base64Avatar: reader.result.toString() }));
    reader.onerror = function (error) {
      console.error('Error: ', error);
    };
  }

  public fileChangeEvent(event: any): void {
    this.imageChangedEvent = event;
  }

  public imageCropped(event: ImageCroppedEvent) {
    this.croppedImage = event.base64;
  }

  public imageLoaded(image: LoadedImage) {
    this.inAvatarEditMode = true;
  }

  public saveCroppedFile() {
    this.store.dispatch(BodyProfileActions.uploadUserAvatarRequest({ base64Avatar: this.croppedImage.toString() }));
    this.inAvatarEditMode = false;
    this.imageChangedEvent = null;
  }

  private getControlValue(controlName: string): any {
    return this.userProfileForm.get(controlName)?.value;
  }
}
