import { Component, Input } from '@angular/core';
import { BrowsingElement } from '../browsing-element';
import { DraftRequestDto } from '../../services/Dtos/DraftRequestDto';

@Component({
    template: ``,
    styleUrls: ['./browsing-item.component.less']
})
export class BrowsingItemComponent {

    @Input()
    element: BrowsingElement;

    @Input()
    index: number;

    @Input()
    model: DraftRequestDto;
}
