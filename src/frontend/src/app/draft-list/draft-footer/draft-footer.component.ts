import { Component, OnInit, Input } from '@angular/core';
import { DraftDto } from '../../services/Dtos/DraftDto';

@Component({
  selector: 'app-draft-footer',
  templateUrl: './draft-footer.component.html',
  styleUrls: ['./draft-footer.component.less']
})
export class DraftFooterComponent implements OnInit {

  @Input()
  draft: DraftDto | undefined;

  ngOnInit() {
  }

}
