import React from 'react'
import { connect } from 'react-redux'
import { Row, Col } from 'react-bootstrap'
import NodeRoles from './nodes/NodeRoles'

const Nodes = props => {
    if (props.nodes.list.length === 0) return (
        <h1>No Nodes Connected</h1>
    )
    if (props.bout.running.length === 0) return (
        <h1>No Bouts Running</h1>
    )

    let emptyGuid = '00000000-0000-0000-0000-000000000000'
    let noBout = props.nodes.list.filter((e, i) => { return e.boutId === emptyGuid })
    if (noBout.length > 0) console.log(noBout)

    return (
        <div>
            <h1>Nodes</h1>
            {noBout.length > 0 ? <div>
                <h2>No Bout Assigned</h2>
                <Row>
                    {noBout.map((e, i) => <Col key={i}>{e.connectionNumber}</Col>)}
                </Row>
            </div> : null}
            {
                props.bout.running.map((bout, boutidx) => <div key={boutidx}>
                    <h2>Bout: {bout.name}</h2>
                    <div>
                        {
                            props.nodes.list.filter((e, i) => { return e.boutId === bout.boutId })
                                .map((node, nodeidx) => <NodeRoles node={node} key={nodeidx} />)
                        }
                    </div>
                </div>
                )
            }
        </div>
    )
};

const mapStateToProps = state => {
    return {
        bout: state.bout,
        nodes: state.nodes
    }
}

const mapDispatchToProps = dispatch => {
    return {

    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Nodes);
