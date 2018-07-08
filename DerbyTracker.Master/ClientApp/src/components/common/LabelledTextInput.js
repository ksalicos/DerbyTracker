import React from 'react'

export default class LabelledTextInput extends React.Component {
    render() {
        let config = this.props.config

        return <div>
            <label>{config.label}</label>
            <input name={config.name} value={config.value} onChange={config.onChange} disabled={config.disabled} />
        </div>;
    }
}