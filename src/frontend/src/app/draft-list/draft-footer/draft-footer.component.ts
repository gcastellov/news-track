import { Component, Input } from '@angular/core';
import { DraftDto } from '../../services/Dtos/DraftDto';

@Component({
  selector: 'app-draft-footer',
  templateUrl: './draft-footer.component.html',
  styleUrls: ['./draft-footer.component.less']
})
export class DraftFooterComponent {

  @Input()
  draft: DraftDto | undefined;

}
