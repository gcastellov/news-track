import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../shared/shared.module';
import { TranslateModule } from '@ngx-translate/core';

import { BrowsingComponent } from './browsing.component';
import { BrowsingItemComponent } from './browsing-item/browsing-item.component';
import { BrowsingPictureComponent } from './browsing-picture/browsing-picture.component';
import { BrowsingTitleComponent } from './browsing-title/browsing-title.component';
import { BrowsingParagraphComponent } from './browsing-paragraph/browsing-paragraph.component';
import { BrowsingDraft } from './browsing-draft/bowsing-draft';
import { BrowsingRelationshipComponent } from './browsing-relationship/browsing-relationship.component';
import { BrowsingRelationshipDraftComponent } from './browsing-relationship-draft/browsing-relationship-draft.component';
import { AuthGuardService } from '../services/Guards/auth-guard.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    NgbModule,
    RouterModule.forChild([
      { path: '', component: BrowsingComponent, canActivate: [AuthGuardService] },
      { path: 'relationship', component: BrowsingRelationshipComponent, canActivate: [AuthGuardService] }
    ]),
    SharedModule
  ],
  declarations: [
    BrowsingComponent,
    BrowsingItemComponent,
    BrowsingPictureComponent,
    BrowsingTitleComponent,
    BrowsingParagraphComponent,
    BrowsingRelationshipComponent,
    BrowsingRelationshipDraftComponent
    ],
  exports: [BrowsingComponent],
  providers: [BrowsingDraft],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class BrowsingModule { }
