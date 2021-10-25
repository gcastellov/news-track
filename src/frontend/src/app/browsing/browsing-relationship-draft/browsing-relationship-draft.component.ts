import { Component, Input, Output, EventEmitter } from '@angular/core';
import { DraftComponent } from '../../draft-list/draft/draft.component';
import { DraftRelationshipDto } from '../../services/Dtos/DraftRelationshipRequestDto';

@Component({
  selector: 'app-browsing-relationship-draft',
  templateUrl: './browsing-relationship-draft.component.html',
  styleUrls: ['./browsing-relationship-draft.component.less']
})
export class BrowsingRelationshipDraftComponent extends DraftComponent {

  @Input()
  isSelected: boolean = false;

  @Output()
  selectionChange: EventEmitter<DraftRelationshipDto> = new EventEmitter<DraftRelationshipDto>();

  onSelectionChange() {
    if (this.draft)
      this.selectionChange.emit(new DraftRelationshipDto(this.draft.id, this.draft.title, this.draft.uri));
  }
}
