import { Injectable } from '@angular/core';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { CanDeactivate } from '@angular/router';

@Injectable({
    providedIn: 'root'
  })
  export class PreventUnsavedChanges implements CanDeactivate<MemberEditComponent> {
    canDeactivate(component: MemberEditComponent) {
        if (component.memberEditForm.dirty) {
            return confirm ('are u sure u want to continue! unsaved changes will e lost');
        }
        return true;
    }


    }
